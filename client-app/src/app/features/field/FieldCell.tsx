import { observer } from "mobx-react";
import "./field.css";
import RowCellNumber from "./RowCellNumber";
import FieldCellRow from "./FieldCellRow";

export default observer(function FieldCell(){
    
    return (
        <>
        <RowCellNumber />
        <FieldCellRow Y={1} number={1}/>
        <FieldCellRow Y={2} number={2}/>
        <FieldCellRow Y={3} number={3}/>
        <FieldCellRow Y={4} number={4}/>
        <FieldCellRow Y={5} number={5}/>
        <FieldCellRow Y={6} number={6}/>
        <FieldCellRow Y={7} number={7}/>
        <FieldCellRow Y={8} number={8}/>
        <FieldCellRow Y={9} number={9}/>
        <FieldCellRow Y={10} number={10}/>
        </>
    )
})