import { observer } from "mobx-react";
import { Button } from "semantic-ui-react";
import NavBar from "../../layout/NavBar";
import FieldCell from "../field/FieldCell";
import FieldForm from "../field/FieldForm";
import "./game.css";

export default observer(function PrepareGame() {
    
    return (
        <>
        <NavBar />
        <div className="prepareGame">
            <div className="field">
                <FieldCell />
            </div>
            <div className="fieldForm">
                <FieldForm />
            </div>
        </div>
        <div className="prepareGameButton">
            <Button color="purple" size="large">I'm ready</Button>
        </div>
        </>
    )
})