import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import { Header, List } from "semantic-ui-react";
import agent from "../../api/agent";
import { GameState } from "../../models/gameState";

export default observer(function GameState() {
    const [gameStates, setGameStates] = useState<GameState[]>([]);

    useEffect(() => {
        agent.GameStates.gameStates().then(response => {
            setGameStates(response);
        })
    }, [])

    return (
        <div>
            <Header as='h2' icon='users' content='GameStates'/>

            <List>
                {gameStates.map(gameState => (
                    <List.Item key={gameState.id}>
                        {gameState.gameStateName}
                    </List.Item>
                ))}
            </List>
        </div>
    )
})