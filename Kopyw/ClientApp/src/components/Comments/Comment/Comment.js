import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';
import formatDate from '../../Shared/Functions/formatDate';
import axios from 'axios';
import classes from './Comment.module.css';

class Comment extends Component {
    constructor(props) {
        super(props);

        this.state = {
            userVote: props.userVote
        };
    }
    render() {
        let plusClasses = [classes.plus]
        let minusClasses = [classes.minus];
        if (this.state.userVote < 0)
            minusClasses.push(classes.minusActive);
        else if (this.state.userVote > 0)
            plusClasses.push(classes.plusActive);
        const plusClassList = plusClasses.join(' ');
        const minusClassList = minusClasses.join(' ');
        let voteButtons;
        let scoreClasses = [classes.score];
        if (this.props.showPlusMinus) {
            voteButtons =
                <>
                    <button className={plusClassList}
                        onClick={this.plusClickHandler}>+1</button>
                    <button className={minusClassList}
                        onClick={this.minusClickHandler}>-1</button>
                </>;
        }
        else {
            voteButtons = null;
            scoreClasses.push(classes.toLeft);
        }
        const scoreClassList = scoreClasses.join(' ');
        const score = this.props.score;
        const scoreSign = score < 0 ? '-' : score > 0 ? '+' : null;
        return (
            <div className={classes.comment}>
                <div className={classes.commentHeader}>
                    <Link className={classes.author} to={`/user/${this.props.authorName}`}>
                        {this.props.authorName}
                    </Link>
                    <span className={classes.time}>{formatDate(this.props.postTime)}</span>
                    {voteButtons}
                    <span className={scoreClassList}>{scoreSign}{this.props.score}</span>
                </div>
                <p className={classes.text}>{this.props.text}</p>
            </div>
        );
    }
    plusClickHandler = () => {
        if (this.state.userVote > 0) {
            this.deleteVote();
            return;
        }
        const data = {
            commentId: this.props.id,
            value: 1
        };
        this.sendVote(data);
    }

    minusClickHandler = () => {
        if (this.state.userVote < 0) {
            this.deleteVote();
            return;
        }
        const data = {
            commentId: this.props.id,
            value: -1
        };
        this.sendVote(data);
    }

    sendVote = data => {
        axios.post("/comment/vote", data)
            .then(r => {
                this.setState({ userVote: r.data.value });
            });
    }

    deleteVote = () => {
        axios.delete(`/comment/vote/${this.props.id}`)
            .then(r => {
                this.setState({ userVote: 0 });
            });
    }
}
Comment.propTypes = {
    id: PropTypes.number.isRequired,
    authorName: PropTypes.string,
    postTime: PropTypes.instanceOf(Date).isRequired,
    score: PropTypes.number,
    userVote: PropTypes.number,
    showPlusMinus: PropTypes.bool
}

export default Comment;