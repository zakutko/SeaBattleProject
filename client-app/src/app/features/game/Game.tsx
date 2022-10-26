import { observer } from "mobx-react";
import FieldCell from "../field/FieldCell";
import SecondPlayerFieldCell from "../field/SecondPlayerFieldCell";
import "./game.css";
import "../field/field.css";
import { useEffect, useState } from "react";
import agent from "../../api/agent";
import GameFieldForm from "./GameFieldForm";

export default observer(function Game(){
    const [numberOfReadyPlayers, setNumberOfReadyPlayers] = useState(0);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            agent.Games.numberOfReadyPlayers(token).then(response => {
                setNumberOfReadyPlayers(response.numberOfReadyPlayers);
            });
        }
        console.log(numberOfReadyPlayers);
    })

    return (
        <>
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
                <GameFieldForm />
            }
        </div>
        </>
    )
})