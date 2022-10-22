import { observer } from "mobx-react";
import RowCell from "./RowCell";
import "./field.css";
import RowCellNumber from "./RowCellNumber";

export default observer(function FieldCell(){
    
    return (
        <>
            <div className="field">
            <RowCellNumber />
            <RowCell number='1' Y={1}/>
            <RowCell number='2' Y={2}/>
            <RowCell number='3' Y={3}/>
            <RowCell number='4' Y={4}/>
            <RowCell number='5' Y={5}/>
            <RowCell number='6' Y={6}/>
            <RowCell number='7' Y={7}/>
            <RowCell number='8' Y={8}/>
            <RowCell number='9' Y={9}/>
            <RowCell number='10' Y={10}/>
        </div>
        </>
    )
})