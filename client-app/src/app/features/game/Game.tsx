import { observer } from "mobx-react";
import FieldCell from "../field/FieldCell";
import SecondPlayerFieldCell from "../field/SecondPlayerFieldCell";
import "./game.css";
import "../field/field.css";
import { useEffect, useState } from "react";
import agent from "../../api/agent";
import GameFieldForm from "./GameFieldForm";
import { Loader, TransitionGroup } from "semantic-ui-react";
import { CircularProgress } from "@mui/material";
import EndOfTheGame from "./EndOfTheGame";

export default observer(function Game(){
    const [numberOfReadyPlayers, setNumberOfReadyPlayers] = useState(0);
    const [loading, setLoading] = useState(true);
    const [isHit, setIsHit] = useState(true);
    const [showComponent, setShowComponent] = useState(false);
    const [isEndOfTheGame, setIsEndOfTheGame] = useState(false);
    const [winnerUserName, setWinnerUserName] = useState("");

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            agent.Games.numberOfReadyPlayers(token).then(response => {
                setNumberOfReadyPlayers(response.numberOfReadyPlayers);
            });
            agent.Games.priopity(token).then(response => {
                setIsHit(response.isHit);
            })
            agent.Games.endOfTheGame(token).then(response => {
                setIsEndOfTheGame(response.isEndOfTheGame);
                setWinnerUserName(response.winnerUserName);
            })
        }

        setInterval(() => {
            setShowComponent(true);
        }, 1000)

        setLoading(false);
    }, [])

    return isEndOfTheGame ? <EndOfTheGame winnerUserName={winnerUserName}/> :
    loading ? <Loader /> :
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
                    {showComponent ? (<TransitionGroup><FieldCell /></TransitionGroup>) : (<CircularProgress color="secondary" />)}
                </div>
                <div className="field">
                    {showComponent ? (<SecondPlayerFieldCell />) : (<CircularProgress color="secondary" />)}
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
})