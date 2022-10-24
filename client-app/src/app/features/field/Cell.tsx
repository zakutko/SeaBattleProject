import { observer } from "mobx-react";
import "./field.css";

export default observer(function Cell(props: any) {
    return (
        <>
        {props.cellState === 1 &&
            <div className="cell"></div>
        }
        {props.cellState === 2 && 
            <div className="cell cellGreen"></div>
        }
        {props.cellState === 5 &&
            <div className="cell cellGrey"></div>
        } 
        </>
    )
})