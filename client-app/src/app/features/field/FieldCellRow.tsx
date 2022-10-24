import { observer } from "mobx-react"
import { useState, useEffect } from "react"
import agent from "../../api/agent"
import { CellList } from "../../models/cellsList"
import Cell from "./Cell"
import CellNumber from "./CellNumber"
import "./field.css"

export default observer(function FieldCellRow(props: any){
    const [cellList, setCellList] = useState<CellList[]>([]);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token){
            agent.Games.cells(token).then(response => {
                setCellList(response);
            });
        }
    }, [])

    return (
        <>
        <div className="rowCell">
            <CellNumber number={props.number}/>
            {cellList.map(cell => (
                <>
                {cell.y === props.Y && 
                    <Cell cellState={cell.cellStateId}/>
                }
                </>
            ))}
        </div>
        </>
    )
})