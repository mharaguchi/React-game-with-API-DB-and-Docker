import React from 'react';
import ReactDOM from 'react-dom';
import {v4 as uuidv4} from 'uuid';
import axios from 'axios';
import './index.css';

function Square(props) {
    return (
        <button className="square" onClick={props.onClick}>
            {props.value}
        </button>
    )
}

class Board extends React.Component {
    renderSquare(i) {
        return (
            <Square
                value={this.props.squares[i]}
                onClick={() => this.props.onClick(i)}
            />
        );
    }

    render() {
        return (
            <div>
                <div className="board-row">
                    {this.renderSquare(0)}
                    {this.renderSquare(1)}
                    {this.renderSquare(2)}
                </div>
                <div className="board-row">
                    {this.renderSquare(3)}
                    {this.renderSquare(4)}
                    {this.renderSquare(5)}
                </div>
                <div className="board-row">
                    {this.renderSquare(6)}
                    {this.renderSquare(7)}
                    {this.renderSquare(8)}
                </div>
            </div>
        );
    }
}

class GameId extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            loadGameId: ""
        };
    }
    handleChange = e => {
        this.setState({ loadGameId: e.target.value})
    }
    handleClick = () => {
        this.props.handleGameIdLoad(this.state.loadGameId);
    }
    render() {
        return(
            <div>
                <div>Current Game ID: {this.props.currentGameId}</div>
                <input type="text" onChange={this.handleChange} value={this.state.loadGameId}/>        
                <button className="gameIdLoad" onClick={this.handleClick}>
                    Load Game
                </button>
            </div>
        );
    }
}

class Game extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            history: [{
                squares: Array(9).fill(null),
            }],
            stepNumber: 0,
            xIsNext: true,
            gameId: uuidv4(),
        };
    }
    jumpTo(step) {
        this.setState({
            stepNumber: step,
            xIsNext: (step % 2) === 0,
            gameId: step > 0 ? this.state.gameId : uuidv4(),
            history: step > 0 ? this.state.history : [{
                squares: Array(9).fill(null),
            }]
        });
    }
    async handleClick(i) {
        const history = this.state.history.slice(0, this.state.stepNumber + 1);
        const current = history[history.length - 1];
        const squares = current.squares.slice();
        if (calculateWinner(squares) || squares[i]) {
            return;
        }
        squares[i] = this.state.xIsNext ? 'X' : 'O';
        this.setState({
            history: history.concat([{
                squares: squares,
            }]),
            stepNumber: history.length,
            xIsNext: !this.state.xIsNext,
        });
        const gameState = {
            id: this.state.gameId,
            gameId: this.state.gameId,
            history: JSON.stringify(history.concat([{
                squares: squares,
            }])),
            turnNumber: history.length
        }
        console.log(gameState);
        await axios.post("http://localhost:19530/TicTacToe/SaveGameState", 
                { gameId: gameState.gameId, history: gameState.history, turnNumber: gameState.turnNumber }, 
                { headers: { 'Content-Type': 'application/json' }}
            ).then((response) => {
                console.log(response.data)
            });
    }
    handleGameIdLoad = async gameIdInput => {
        console.log(gameIdInput);
        var that = this;
        await axios.get("http://localhost:19530/TicTacToe/GetGameState",
                { params: { gameId: gameIdInput }},
                { headers: { 'Content-Type': 'application/json' }}
            ).then(function(response) {
                console.log("this response");
                console.log(that);
                    const gameState = {
                        gameId: response.data.gameId,
                        turnNumber: response.data.turnNumber,
                        history: response.data.history
                    };
                    that.setState({
                        stepNumber: gameState.turnNumber,
                        xIsNext: (gameState.turnNumber % 2) === 0,
                        gameId: gameState.gameId,
                        history: JSON.parse(gameState.history)
                    });
                // }).then((gameState) => {
                //     console.log("this gamestate");
                //     console.log(this);
                //     console.log(gameState);
                //     console.log("calling loadgame");
                //     }.bind(this));
                //     console.log("here");
                //console.log(response);
            }).catch((error) => {
                console.log(error);
                alert("Could not load game " + gameIdInput + ".");
            });
    }
    render() {
        const history = this.state.history;
        const current = history[this.state.stepNumber];
        const winner = calculateWinner(current.squares);

        const moves = history.map((step, move) => {
            const desc = move ?
                'Go to move #' + move :
                'New game';
            return (
                <li key={move}>
                    <button onClick={() => this.jumpTo(move)}>{desc}</button>
                </li>
            );
        });

        let status;
        if (winner) {
            status = 'Winner: ' + winner;
        }
        else {
            status = 'Next player: ' + (this.state.xIsNext ? 'X' : 'O');
        }
        return (
            <div className="game">
                <div className="game-board">
                    <Board
                        squares={current.squares}
                        onClick={(i) => this.handleClick(i)}
                    />
                </div>
                <div className="game-info">
                    <GameId currentGameId={this.state.gameId} handleGameIdLoad={this.handleGameIdLoad} />
                    <div>{status}</div>
                    <ol>{moves}</ol>
                </div>
            </div>
        );
    }
}

// ========================================

ReactDOM.render(
    <Game />,
    document.getElementById('root')
);

function calculateWinner(squares) {
    const lines = [
        [0, 1, 2],
        [3, 4, 5],
        [6, 7, 8],
        [0, 3, 6],
        [1, 4, 7],
        [2, 5, 8],
        [0, 4, 8],
        [2, 4, 6],
    ];
    for (let i = 0; i < lines.length; i++) {
        const [a, b, c] = lines[i];
        if (squares[a] && squares[a] === squares[b] && squares[a] === squares[c]) {
            return squares[a];
        }
    }
    return null;
}