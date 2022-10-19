import { observer } from "mobx-react";
import RowCell from "./RowCell";
import "./field.css";
import RowCellNumber from "./RowCellNumber";


export default observer(function FieldCell(){
    return (
        <>
        <div className="field">
            <RowCellNumber />
            <RowCell number='1'/>
            <RowCell number='2'/>
            <RowCell number='3'/>
            <RowCell number='4'/>
            <RowCell number='5'/>
            <RowCell number='6'/>
            <RowCell number='7'/>
            <RowCell number='8'/>
            <RowCell number='9'/>
            <RowCell number='10'/>
        </div>
        </>
    )
})