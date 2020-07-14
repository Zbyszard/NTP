import React, { Component } from 'react';
import AuthorizedRender from '../api-authorization/AuthorizedRender';
import AuthContext from '../api-authorization/AuthContext';
import CommentForm from './CommentForm';
import Comment from './Comment/Comment';
import PropTypes from 'prop-types';
import axios from 'axios';
import classes from './CommentSection.module.css';
import Warning from '../Shared/Warning/Warning';

class CommentSection extends Component {
    constructor(props) {
        super(props);
        this.state = {
            show: props.show,
            isLoading: true,
            comments: [],
            commentIdToBeDeleted: null,
            formBlocked: false,
            formKey: new Date().getTime()
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

    sendComment = newComment => {
        this.setState({ formBlocked: true });
        axios.post("/comment/add", newComment).then(r => {
            const comment = r.data;
            this.setState(state => {
                this.setState({
                    formKey: new Date().getTime(),
                    formBlocked: false
                });
                return { comments: [...state.comments, comment] };
            });
        });
    }

    commentEditedHandler = comment => {
        axios.put("/comment/edit", comment)
            .then(r => {
                if (r.status === 200) {
                    let updatedComment = r.data;
                    let comments = this.state.comments;
                    let index = comments.findIndex(c => c.id === updatedComment.id);
                    if (index !== -1) {
                        comments[index] = updatedComment;
                        this.setState({ comments: comments });
                    }
                }
            });
    }

    commentDeleteClickHandler = commentId => {
        this.setState({ commentIdToBeDeleted: commentId });
    }

    commentDeleteHandler = () => {
        axios.delete(`/comment/delete/${this.state.commentIdToBeDeleted}`)
            .then(r => {
                if (r.status === 200) {
                    this.requestData();
                }
            })
            .finally(() => {
                this.setState({ commentIdToBeDeleted: null });
            });
    }

    cancelCommentDelete = () => {
        this.setState({ commentIdToBeDeleted: null });
    }

    render() {
        let comments;
        if (this.state.isLoading)
            comments = <div>Loading...</div>;
        else if (this.state.comments.length === 0)
            comments = <div>No comments</div>;
        else
            comments = this.state.comments.map(c =>
                <AuthContext.Consumer key={c.id}>
                    {context => <Comment text={c.text}
                        postId={c.postId}
                        deleted={c.deleted}
                        id={c.id}
                        authorName={c.authorName}
                        authorId={c.authorId}
                        postTime={new Date(c.postTime)}
                        score={c.score}
                        userVote={c.userVote}
                        showPlusMinus={context.authorized && context.userName !== c.authorName}
                        userAuthorized={context.authorized}
                        userName={context.userName}
                        editCallback={this.commentEditedHandler}
                        deleteCallback={this.commentDeleteClickHandler} />
                    }
                </AuthContext.Consumer>);
        let containerClasses = [classes.container];
        if (!this.props.show)
            containerClasses.push(classes.hidden);
        let containerClassList = containerClasses.join(' ');
        let deleteWarning = null;
        if (this.state.commentIdToBeDeleted) {
            deleteWarning =
                <Warning
                    message="Are you sure you want to delete this comment?"
                    confirmCallback={this.commentDeleteHandler}
                    cancelCallback={this.cancelCommentDelete} />;
        }
        return (
            <>
                {deleteWarning}
                <section>
                    <div className={containerClassList}>
                        <AuthorizedRender>
                            <CommentForm key={this.state.formKey}
                                isBlocked={this.state.formBlocked}
                                postId={this.props.postId}
                                submitCallback={this.sendComment} />
                        </AuthorizedRender>
                        <div className={classes.commentContainer}>
                            {comments}
                        </div>
                    </div>
                </section>
            </>
        );
    }

}

CommentSection.propTypes = {
    postId: PropTypes.number.isRequired,
    comments: PropTypes.arrayOf(PropTypes.object)
}

export default CommentSection;