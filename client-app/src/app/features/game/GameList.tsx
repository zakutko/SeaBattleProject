import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Button, Label, Table } from "semantic-ui-react";
import agent from "../../api/agent";
import NavBar from "../../layout/NavBar";
import { GameList } from "../../models/gameList";
import "./game.css";

export default observer(function GameList() {
    const [gameList, setGameList] = useState<GameList[]>([]);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token){
            agent.Games.games(token).then(response => {
                setGameList(response);
            });
        }
    }, [])

    const onClick = () => {
        const token = localStorage.getItem('token');
        if(token){
            agent.Games.createGame(token);
        }
    }

    const onClickJoin = (id: number) => {
        const token = localStorage.getItem('token');
        if(token){
            agent.Games.joinSecondPlayer(id, token);
        }
    }
    
    return (
        <>
        <NavBar />
        <div className="gameList">
            <Button onClick={onClick} as={Link} to="/prepareGame" color="purple">Create Game</Button>
            <Table celled>
                <Table.Header>
                    <Table.Row>
                        <Table.HeaderCell>Game</Table.HeaderCell>
                        <Table.HeaderCell>Owner</Table.HeaderCell>
                        <Table.HeaderCell>Game State</Table.HeaderCell>
                        <Table.HeaderCell>Number of Players</Table.HeaderCell>
                        <Table.HeaderCell></Table.HeaderCell>
                    </Table.Row>
                </Table.Header>

                <Table.Body>
                    {gameList.map(game => (
                        <Table.Row key={game.id}>
                            <Table.Cell>
                                {game.id}
                            </Table.Cell>
                            <Table.Cell>
                                {game.firstPlayer}
                            </Table.Cell>
                            <Table.Cell>
                                {game.gameState}
                            </Table.Cell>
                            <Table.Cell>
                                {game.numberOfPlayers} of 2
                            </Table.Cell>
                            {game.numberOfPlayers === 2 ? (
                                <Table.Cell>
                                    <Label>The game already started!</Label>
                                </Table.Cell>
                            ) : (
                                <Table.Cell>
                                    <Button as={Link} to="/prepareGame" onClick={() => onClickJoin(game.id)}>Join the game</Button>
                                </Table.Cell>
                            )}
                        </Table.Row>
                    ))}
                </Table.Body>
            </Table>
        </div>
        </>
    )
})