import axios, { AxiosResponse } from "axios";
import { GameList } from "../models/gameList";
import { GameState } from "../models/gameState";
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

const GameStates = {
    gameStates: () => request.get<GameState[]>('/GameState')
}

const Games = {
    games: (token: string) => request.get<GameList[]>(`/Game?token=${token}`),
    createGame: (token: string) => request.get<void>(`Game/createGame?token=${token}`),
    joinSecondPlayer: (id: number, token: string) => request.get<void>(`Game/joinSecondPlayer?id=${id}&&username=${token}`),
}

const Account = {
    current: () => request.get<User>('/Account'),
    login: (user: UserFormValues) => request.post<User>('/Account/login', user),
    register: (user: UserFormValues) => request.post<User>('/Account/register', user),
}

const agent = {
    GameStates,
    Games,
    Account
}

export default agent;