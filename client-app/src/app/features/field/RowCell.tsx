import { observer } from "mobx-react"
import Cell from "./Cell"
import CellNumber from "./CellNumber"
import "./field.css"

export default observer(function RowCell(props: any){
    return (
        <>
        <div className="rowCell">
            <CellNumber number={props.number}/>
            <Cell />
            <Cell />
            <Cell />
            <Cell />
            <Cell />
            <Cell />
            <Cell />
            <Cell />
            <Cell />
            <Cell />
        </div>
        </>
    )
})