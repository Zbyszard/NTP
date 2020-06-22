import React from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';
import formatDate from '../../Shared/Functions/formatDate';
import classes from './Comment.module.css';

const Comment = props => {
    let scoreClasses = [classes.score];
    let voteButtons;
    if (props.showPlusMinus) {
        voteButtons =
            <>
                <button className={classes.plus}>+1</button>
                <button className={classes.minus}>-1</button>
            </>;
    }
    else
    {
        voteButtons = null;
        scoreClasses.push(classes.toLeft);
    }
    const scoreClassList = scoreClasses.join(' ');
    return (
        <div className={classes.comment}>
            <div className={classes.commentHeader}>
                <Link className={classes.author} to={`/user/${props.authorName}`}>
                    {props.authorName}
                </Link>
                <span className={classes.time}>{formatDate(props.postTime)}</span>
                {voteButtons}
                <span className={scoreClassList}>{props.score}</span>
            </div>
            <p className={classes.text}>{props.text}</p>
        </div>
    );
}

Comment.propTypes = {
    id: PropTypes.number.isRequired
}

export default Comment;