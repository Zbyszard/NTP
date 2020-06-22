import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';
import CommentSection from '../../../Comments/CommentSection';
import formatDate from '../../../Shared/Functions/formatDate';
import axios from 'axios';
import classes from './Post.module.css';

class Post extends Component {
    constructor(props) {
        super(props);
        this.state = {
            userVote: props.userVote,
            score: props.score,
            showComments: false
        };
    }
    render() {
        let plusClasses = [classes.plus];
        if (this.state.userVote)
            plusClasses.push(classes.active);
        const plusClassList = plusClasses.join(' ');
        const plus1 = this.props.showPlus ?
            <button className={plusClassList}
                onClick={this.plusClickHandler}>+1</button> :
            null;
        return (
            <>
                <div className={classes.post}>
                    <Link className={classes.author} to={`/user/${this.props.author}`}>
                        {this.props.author}
                    </Link>
                    <h1 className={classes.title}>
                        <Link to={`/post/${this.props.id}`}>
                            {this.props.title}
                        </Link>
                    </h1>
                    <p className={classes.time}>
                        {formatDate(this.props.postTime)}
                    </p>
                    <p className={classes.text}>
                        {this.props.text}
                    </p>
                    <div className={classes.actionSection}>
                        <span className={classes.score}>+{this.state.score}</span>
                        {plus1}
                        <button className={classes.commentCount}
                            onClick={this.toggleComments}>
                            {this.props.commentCount} comments
                        </button>
                    </div>
                    <CommentSection postId={this.props.id}
                        show={this.state.showComments}
                        showPlusMinus={this.props.showPlus} />
                </div>

            </>
        );
    }

    toggleComments = () => {
        this.setState(state => {
            return { showComments: !state.showComments };
        });
    }

    plusClickHandler = () => {
        if (this.state.userVote)
            this.deleteVote();
        else
            this.vote();
    }

    vote = () => {
        const data = { postId: this.props.id };
        axios.post("/post/vote", data).then(response => {
            const r = response.data;
            this.setState({ userVote: true });
        });
    }

    deleteVote = () => {
        const data = { postId: this.props.id };
        axios.delete(`/post/vote/${this.props.id}`).then(response => {
            this.setState({ userVote: false });
        });
    }
}

Post.propTypes = {
    id: PropTypes.number.isRequired,
    author: PropTypes.string,
    title: PropTypes.string.isRequired,
    text: PropTypes.string.isRequired,
    postTime: PropTypes.instanceOf(Date).isRequired,
    score: PropTypes.number.isRequired,
    commentCount: PropTypes.number.isRequired,
    showPlus: PropTypes.bool,
    userAuthenticated: PropTypes.bool
}

export default Post;