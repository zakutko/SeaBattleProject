import { observer } from "mobx-react";
import { useState } from "react";
import { useNavigate } from "react-router";
import { Button } from "semantic-ui-react";
import agent from "../../api/agent";
import FieldCell from "../field/FieldCell";
import FieldForm from "../field/FieldForm";
import "./game.css";

export default observer(function PrepareGame() {
    const navigate = useNavigate();
    const [message, setMessage] = useState("");

    function onClick() {
        const token = localStorage.getItem('token');
        if (token) {
            agent.Games.firstPlayerReady(token).then(response => {
                setMessage(response.message);
            });
        }
        console.log(message);
        navigate("/game");
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
            <Button onClick={onClick} color="purple" size="large">I'm ready</Button>
        </div>  
        </>
    )
})