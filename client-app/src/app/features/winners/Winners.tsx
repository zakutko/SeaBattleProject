import { observer } from "mobx-react";
import "./winners.css";

export default observer(function Winners() {
    

    return (
        <>
         <div className="winner">
                <h1 className="winner-title">Top 3 players:</h1>
                <div className="winner-categories">
                    <div className="third-place">
                        <h1 className="third-place-title">3 Place</h1>
                        <h2 className="third-place-player-name">Player name</h2>
                        <h2 className="third-place-number-of-wins">10</h2>
                    </div>
                    <div className="first-place">
                        <h1 className="first-place-title">1 Place</h1>
                        <h2 className="first-place-player-name">Player name</h2>
                        <h2 className="first-place-number-of-wins">10</h2>
                    </div>
                    <div className="second-place">
                        <h1 className="second-place-title">2 Place</h1>
                        <h2 className="second-place-player-name">Player name</h2>
                        <h2 className="second-place-number-of-wins">10</h2>
                    </div>
                </div>
            </div>
        </>
    )
})