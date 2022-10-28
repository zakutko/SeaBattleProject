export interface IsHit {
    isHit: boolean
}

export interface IsPlayerReady {
    message: string;
}

export interface IsTwoPlayersReady {
    numberOfReadyPlayers: number
}

export interface IsEndOfTheGame {
    isEndOfTheGame: boolean
    winnerUserName: string
}