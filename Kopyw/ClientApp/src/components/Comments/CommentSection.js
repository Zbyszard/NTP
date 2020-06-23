import React, { Component } from 'react';
import AuthorizedRender from '../api-authorization/AuthorizedRender';
import AuthContext from '../api-authorization/AuthContext';
import CommentForm from './CommentForm';
import Comment from './Comment/Comment';
import PropTypes from 'prop-types';
import axios from 'axios';
import classes from './CommentSection.module.css';

class CommentSection extends Component {
    constructor(props) {
        super(props);
        this.state = {
            show: props.show,
            isLoading: true,
            comments: []
            /*comments: [{
                id: 0,
                authorName: "Andrzej",
                text: "fajen",
                score: -1,
                userVote: 1
            },
            {
                id: 1,
                authorName: "Akwarelista",
                text: ":(",
                score: 20,
                userVote: -1
            },
            {
                id: 2,
                authorName: "QWEASD",
                text: "pzdr",
                score: -89,
                userVote: -1
            },
            {
                id: 3,
                authorName: "Zagłoba",
                text: "+1 byczq",
                score: 45845,
                userVote: 0
            }]*/
        }
    }

    componentDidMount = () => {
        this.requestData();
    }

    requestData = () => {
        axios.get(`/comment/${this.props.postId}`)
            .then(r => {
                const comments = r.data;
                this.setState({
                    comments: comments,
                    isLoading: false
                });
            })
            .catch(() => {
                this.setState({ isLoading: false });
            });
    }

    commentAddedCallback = comment => {
        this.setState(state => {
            return { comments: [comment, ...state.comments] };
        });
    }

    render() {
        if (!this.props.show)
            return null;

        let comments;
        if (this.state.isLoading)
            comments = <div>Loading...</div>;
        else if (this.state.comments.length == 0)
            comments = <div>No comments yet.</div>;
        else
            comments = this.state.comments.map(c =>
                <AuthContext>
                    {context => <Comment text={c.text}
                        id={c.id} key={c.id}
                        authorName={c.authorName}
                        postTime={new Date(c.postTime)}
                        score={c.score}
                        userVote={c.userVote}
                        showPlusMinus={context.authorized && context.userName !== c.authorName} />
                    }
                </AuthContext>);
        return (
            <section>
                <div className={classes.container}>
                    <AuthorizedRender>
                        <CommentForm postId={this.props.postId}
                            onCommentPost={this.commentAddedCallback} />
                    </AuthorizedRender>
                    <div className={classes.commentContainer}>
                        {comments}
                    </div>
                </div>
            </section>
        );
    }
}

CommentSection.propTypes = {
    postId: PropTypes.number.isRequired,
    comments: PropTypes.arrayOf(PropTypes.object)
}

export default CommentSection;