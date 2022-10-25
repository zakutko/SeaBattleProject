import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { Button } from "semantic-ui-react";
import agent from "../../api/agent";
import FieldCell from "../field/FieldCell";
import FieldForm from "../field/FieldForm";
import "./game.css";

export default observer(function PrepareGame() {
    const [numberOfReadyPlayers, setNumberOfReadyPlayers] = useState(0);
    const navigate = useNavigate();
    const token = localStorage.getItem('token');

    useEffect(() => {
        if (numberOfReadyPlayers === 2){
            navigate("/game");
        }
    })

    const onClick = () => {
        if (token){
            agent.Games.numberOfReadyPlayers(token).then(response => {
                setNumberOfReadyPlayers(response.numberOfReadyPlayers);
            });
        }
    }

    return (
        <>
        <div className="prepareGame">
            <div className="field">
                <FieldCell />
            </div>
            <div className="fieldForm">
                <FieldForm />
            </div>
        </div>
        <div className="prepareGameButton">
            {numberOfReadyPlayers === 0 &&
                <Button onClick={onClick} color="purple" size="large">I'm ready</Button>
            }
            {numberOfReadyPlayers === 1 &&
                <h1>Waiting an opponent!</h1>
            }
        </div>  
        </>
    )
})