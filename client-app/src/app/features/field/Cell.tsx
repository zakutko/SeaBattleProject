import { observer } from "mobx-react";
import "./field.css";

export default observer(function Cell(props: any) {
    return (
        <div className="cell" >
            <p>{props.cellState}</p>
        </div>
    )
})