import React, { Component } from 'react';
import AuthorizedRender from '../api-authorization/AuthorizedRender';
import CommentForm from './CommentForm';
import Comment from './Comment/Comment';
import PropTypes from 'prop-types';

import classes from './CommentSection.module.css';

class CommentSection extends Component {
    constructor(props) {
        super(props);
        this.state = {
            show: props.show,
            comments: [{
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
            }]
        }
    }

    render() {
        if (!this.props.show)
            return null;
        const comments = this.state.comments.map(c =>
            <Comment text={c.text}
                id={c.id} key={c.id}
                authorName={c.authorName}
                postTime={!c.postTime ? new Date() : c.postTime}
                score={c.score}
                userVote={c.userVote}
                showPlusMinus={this.props.showPlusMinus} />);
        return (
            <section>
                <div className={classes.container}>
                    <AuthorizedRender>
                        <CommentForm />
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