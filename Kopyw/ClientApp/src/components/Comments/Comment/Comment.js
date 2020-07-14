import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';
import formatDate from '../../Shared/Functions/formatDate';
import Button from '../../Shared/Button/Button';
import axios from 'axios';
import classes from './Comment.module.css';
import CommentForm from '../CommentForm';

class Comment extends Component {
    constructor(props) {
        super(props);

        this.state = {
            userVote: props.userVote,
            editMode: false
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
        if (this.props.showPlusMinus && !this.props.deleted) {
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
        let ownCommentActions = null;
        if (this.props.userAuthorized && this.props.userName === this.props.authorName)
            ownCommentActions =
                <>
                    <Button fontSize="0.7rem" onClick={this.editClickHandler}>Edit</Button>
                    <Button fontSize="0.7rem" onClick={this.deleteClickHandler}>Delete</Button>
                </>
        let commentBody =
            <p className={classes.text}>
                {!this.props.deleted ? this.props.text : "[comment deleted]"}
            </p>;
        if (this.state.editMode) {
            commentBody =
                <CommentForm
                    postId={this.props.postId}
                    submitCallback={this.editSubmitHandler}
                    cancelCallback={this.cancelEdit}
                    text={this.props.text} />
        }
        let userLink =
            <Link className={classes.author} to={`/user/${this.props.authorName}`}>
                {this.props.authorName}
            </Link>;
        if (this.props.deleted)
            userLink = <span className={classes.author}>[comment deleted]</span>
        return (
            <div className={classes.comment}>
                <div className={classes.commentHeader}>
                    {userLink}
                    <span className={classes.time}>{formatDate(this.props.postTime)}</span>
                    {ownCommentActions}
                    {voteButtons}
                    <span className={scoreClassList}>{this.props.score}</span>
                </div>
                {commentBody}
            </div>
        );
    }

    editClickHandler = () => {
        this.setState({ editMode: true });
    }

    editSubmitHandler = comment => {
        comment.id = this.props.id;
        comment.authorId = this.props.authorId;
        comment.authorName = this.props.authorName;
        this.props.editCallback(comment);
        this.setState({ editMode: false });
    }

    deleteClickHandler = () => {
        this.props.deleteCallback(this.props.id);
    }

    cancelEdit = () => {
        this.setState({ editMode: false });
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
    postId: PropTypes.number.isRequired,
    id: PropTypes.number.isRequired,
    authorName: PropTypes.string,
    authorId: PropTypes.string,
    postTime: PropTypes.instanceOf(Date).isRequired,
    score: PropTypes.number,
    userVote: PropTypes.number,
    showPlusMinus: PropTypes.bool,
    userAuthorized: PropTypes.bool.isRequired,
    userName: PropTypes.string.isRequired,
    editCallback: PropTypes.func,
    deleteCallback: PropTypes.func
}

export default Comment;