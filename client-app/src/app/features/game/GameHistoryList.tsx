import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material"
import { observer } from "mobx-react"
import { useEffect, useState } from "react"
import agent from "../../api/agent"
import NavBar from "../../layout/NavBar"
import { GameHistoryList } from "../../models/gameHistoryList"
import Winners from "../winners/Winners"
import "./game.css";

export default observer(function GameHistoryList() {
    const [gameHistoryList, setGameHistoryList] = useState<GameHistoryList[]>([])

    useEffect(() => {
        agent.GameHistories.gameHistories().then(response => {
            setGameHistoryList(response);
        });
        const interval = setInterval(() => {
            agent.GameHistories.gameHistories().then(response => {
                setGameHistoryList(response);
            });
        }, 1000)
        return () => clearInterval(interval);
    }, [])

    return (
        <>
        <NavBar />

        <Winners />
        
        <div className="gameHistoryTable">
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell><b>Game Id:</b></TableCell>
                            <TableCell align="right"><b>First player:</b></TableCell>
                            <TableCell align="right"><b>Second player:</b></TableCell>
                            <TableCell align="right"><b>Game state:</b></TableCell>
                            <TableCell align="right"><b>Winner:</b></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                    {gameHistoryList.map((gameHistory) => (
                        <TableRow
                        key={gameHistory.id}
                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                        >
                        <TableCell component="th" scope="row">
                            {gameHistory.gameId}
                        </TableCell>
                        <TableCell align="right" component="th" scope="row">
                            {gameHistory.firstPlayerName}
                        </TableCell>
                        <TableCell align="right" component="th" scope="row">
                            {gameHistory.secondPlayerName}
                        </TableCell>
                        <TableCell align="right" component="th" scope="row">
                            {gameHistory.gameStateName}
                        </TableCell>
                        <TableCell align="right" component="th" scope="row">
                            {gameHistory.winnerName}
                        </TableCell>
                        </TableRow>
                    ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </div>
        </>
    )
})