import { observer } from "mobx-react"
import { useEffect, useState } from "react"
import agent from "../../api/agent"
import { CellList } from "../../models/cellsList"
import Cell from "./Cell"
import CellNumber from "./CellNumber"
import "./field.css"

export default observer(function RowCell(props: any){
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
        {cellList.map(cell => (
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
        ))}
        </>
    )
})