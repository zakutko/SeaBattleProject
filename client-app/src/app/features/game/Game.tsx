import { observer } from "mobx-react";
import FieldCell from "../field/FieldCell";
import SecondPlayerFieldCell from "../field/SecondPlayerFieldCell";
import "./game.css";
import "../field/field.css";
import { useEffect, useState, useTransition } from "react";
import agent from "../../api/agent";
import GameFieldForm from "./GameFieldForm";
import EndOfTheGame from "./EndOfTheGame";

export default observer(function Game(){
    const [numberOfReadyPlayers, setNumberOfReadyPlayers] = useState(0);
    const [isHit, setIsHit] = useState(true);
    const [isEndOfTheGame, setIsEndOfTheGame] = useState(false);
    const [winnerUserName, setWinnerUserName] = useState("");
    const [isPending, startTransition] = useTransition();

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            startTransition(() => {
                agent.Games.numberOfReadyPlayers(token).then(response => {
                    setNumberOfReadyPlayers(response.numberOfReadyPlayers);
                });
                agent.Games.priopity(token).then(response => {
                    setIsHit(response.isHit);
                })
                agent.Games.endOfTheGame(token).then(response => {
                    setWinnerUserName(response.winnerUserName);
                    setIsEndOfTheGame(response.isEndOfTheGame);
                })
            })
        }
    }, [])

    return isEndOfTheGame ? (<EndOfTheGame winnerUserName={winnerUserName}/>) : (
        <>
        <div>
            <div className="help-colors">
                <div className="block color-purple">
                    <p>Miss</p>
                </div>
                <div className="block color-orange">
                    <p>Hit</p>
                </div>
                <div className="block color-red">
                    <p>Destroyed</p>
                </div>
            </div>
            <div className="game">
                <div className="field">
                    <FieldCell />
                </div>
                <div className="field">
                    <SecondPlayerFieldCell />
                </div>
            </div>
            <div className="gameFieldForm">
                {numberOfReadyPlayers === 1 &&
                    <h1>Waiting an opponent!</h1>
                }
                {numberOfReadyPlayers === 2 &&
                    <>
                    {isHit ? (<GameFieldForm />) : (<div><h1>Enemy fire!</h1></div>)}
                    </>
                }
            </div>
        </div>
        </>
    )
})