import { observer } from "mobx-react";
import FieldCell from "../field/FieldCell";
import SecondPlayerFieldCell from "../field/SecondPlayerFieldCell";
import "./game.css";
import "../field/field.css";

export default observer(function Game(){
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
            
        </div>
        </>
    )
})