import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';
import classes from './Post.module.css';

class Post extends Component {
    constructor(props) {
        super(props);
        this.state = {
            comments: []
        };
    }
    render() {
        return (
            <div className={classes.post}>
                <h2 className={classes.author}>
                    <Link>
                        {this.props.author}
                    </Link>
                </h2>
                <h1 className={classes.title}>
                    <Link>
                        {this.props.title}
                    </Link>
                </h1>
                <p className={classes.text}>
                    {this.props.text}
                </p>
            </div>
        );
    }
}

Post.propTypes = {
    id: PropTypes.number.isRequired,
    author: PropTypes.string,
    title: PropTypes.string.isRequired,
    text: PropTypes.string.isRequired,
    score: PropTypes.number.isRequired,
    commentCount: PropTypes.number.isRequired,
    userVote: PropTypes.bool,
    userAuthenticated: PropTypes.bool
}

export default Post;