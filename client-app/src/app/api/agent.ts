import axios, { AxiosResponse } from "axios";
import { CellList } from "../models/cellsList";
import { IsEndOfTheGame, IsHit, IsPlayerReady, IsTwoPlayersReady } from "../models/checks";
import { GameList } from "../models/gameList";
import { Ship, ShipFormValues } from "../models/ship";
import { ShootFormValues } from "../models/shoot";
import { User, UserFormValues } from "../models/user";
import { store } from "../stores/store";

axios.defaults.baseURL = 'https://localhost:5001/api';

axios.defaults.headers.common['Authorization'] = `Bearer ${store.commonStore.token}`;

const responceBody = <T> (response: AxiosResponse<T>) => response.data;

const request = {
    get: <T> (url: string) => axios.get<T>(url).then(responceBody),
    post: <T> (url: string, body: {}) => axios.post<T>(url, body).then(responceBody),
    put: <T> (url: string, body: {}) => axios.put<T>(url, body).then(responceBody),
    delete: <T> (url: string) => axios.delete<T>(url).then(responceBody),
}

const Games = {
    games: (token: string) => request.get<GameList[]>(`/Game?token=${token}`),
    createGame: (token: string) => request.get<void>(`Game/createGame?token=${token}`),
    joinSecondPlayer: (id: number, token: string) => request.get<void>(`Game/joinSecondPlayer?gameId=${id}&&token=${token}`),
    createShipOnField: (ship: ShipFormValues) => request.post<Ship>('/Game/prepareGame/createShipOnField', ship),
    cells: (token: string) => request.get<CellList[]>(`/Game/prepareGame?token=${token}`),
    numberOfReadyPlayers: (token: string) => request.get<IsTwoPlayersReady>(`/Game/isTwoPlayersReady?token=${token}`),
    firstPlayerReady: (token: string) => request.get<IsPlayerReady>(`/Game/isPlayerReady?token=${token}`),
    secondPlayerCells: (token: string) => request.get<CellList[]>(`/Game/game/secondPlayerCells?token=${token}`),
    fire: (shoot: ShootFormValues) => request.post<void>('Game/game/fire', shoot),
    priopity: (token: string) => request.get<IsHit>(`Game/game/priority?token=${token}`),
    endOfTheGame: (token: string) => request.get<IsEndOfTheGame>(`Game/game/endOfTheGame?token=${token}`),
    clearingDb: (token: string) => request.get<void>(`Game/game/clearingDb?token=${token}`)
}

const Account = {
    current: () => request.get<User>('/Account'),
    login: (user: UserFormValues) => request.post<User>('/Account/login', user),
    register: (user: UserFormValues) => request.post<User>('/Account/register', user),
}

const agent = {
    Games,
    Account
}

export default agent;