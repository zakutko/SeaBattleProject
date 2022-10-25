import { observer } from "mobx-react";
import { useState, useEffect } from "react";
import agent from "../../api/agent";
import { CellList } from "../../models/cellsList";
import CellNumber from "./CellNumber";
import SecondPlayerCell from "./SecondPlayerCell";

export default observer(function SecondPlayerFieldCellRow(props: any){
    const [cellList, setCellList] = useState<CellList[]>([]);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token){
            agent.Games.secondPlayerCells(token).then(response => {
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
                    <SecondPlayerCell cellState={cell.cellStateId}/>
                }
                </>
            ))}
        </div>
        </>
    )
})