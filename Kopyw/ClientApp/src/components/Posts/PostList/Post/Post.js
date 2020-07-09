import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { follow, unfollow } from '../../../Shared/FollowApiCalls/FollowApiCalls';
import PropTypes from 'prop-types';
import CommentSection from '../../../Comments/CommentSection';
import ContextMenu from '../../../Shared/ContextMenu/ContextMenu';
import ContextMenuItem from '../../../Shared/ContextMenu/ContextMenuItem';
import formatDate from '../../../Shared/Functions/formatDate';
import classes from './Post.module.css';

class Post extends Component {
    constructor(props) {
        super(props);
        this.state = {
            showComments: false
        };
    }
    render() {
        let plusClasses = [classes.plus];
        if (this.props.userVote)
            plusClasses.push(classes.active);
        const plusClassList = plusClasses.join(' ');
        const plus1 = this.props.showPlus ?
            <button className={plusClassList}
                onClick={this.plusClickHandler}>+1</button> :
            null;
        let menu = null;
        if (this.props.userAuthorized) {
            const menuItems = this.getMenuItems();
            menu =
                <div className={classes.menuContainer}>
                    <ContextMenu>
                        {menuItems}
                    </ContextMenu>
                </div>;
        }
        return (
            <>
                <div className={classes.post}>
                    <div className={classes.header}>
                        <Link className={classes.author} to={`/user/${this.props.author}`}>
                            {this.props.author}
                        </Link>
                        {menu}
                    </div>
                    <h1 className={classes.title}>
                        {/* <Link to={`/post/${this.props.id}`}>
                            {this.props.title}
                        </Link> */}
                        <div>{this.props.title}</div>
                    </h1>
                    <p className={classes.time}>
                        {formatDate(this.props.postTime)}
                    </p>
                    <p className={classes.text}>
                        {this.props.text}
                    </p>
                    <div className={classes.actionSection}>
                        <span className={classes.score}>+{this.props.score}</span>
                        {plus1}
                        <button className={classes.commentCount}
                            onClick={this.toggleComments}>
                            {this.props.commentCount} {this.props.commentCount === 1 ? "comment" : "comments"}
                        </button>
                    </div>
                    <CommentSection postId={this.props.id}
                        show={this.state.showComments} />
                </div>

            </>
        );
    }

    getMenuItems = () => {
        const items = [];
        let index = 0;
        if (this.props.author === this.props.userName) {
            items.push(<ContextMenuItem key={index++}>Edit</ContextMenuItem>);
            items.push(<ContextMenuItem key={index++}>Delete</ContextMenuItem>);
        }
        else
            items.push(<ContextMenuItem key={index++} clickCallback={this.followItemClicked}>
                {this.props.followingAuthor ? "Unfollow user" : "Observe user"}
            </ContextMenuItem>);
        return items;
    }

    followItemClicked = () => {
        if (!this.props.followingAuthor)
            follow(this.props.authorId).then(() => this.props.followCallback());
        else
            unfollow(this.props.authorId).then(() => this.props.followCallback());

    }

    toggleComments = () => {
        this.setState(state => {
            return { showComments: !state.showComments };
        });
    }

    plusClickHandler = () => {
        if (this.props.userVote)
            this.deleteVote();
        else
            this.vote();
    }

    vote = () => {
        this.props.voteCallback(this.props.id);
    }

    deleteVote = () => {
        this.props.deleteVoteCallback(this.props.id);
    }
}

Post.propTypes = {
    id: PropTypes.number.isRequired,
    author: PropTypes.string,
    authorId: PropTypes.string,
    title: PropTypes.string.isRequired,
    text: PropTypes.string.isRequired,
    postTime: PropTypes.instanceOf(Date).isRequired,
    score: PropTypes.number.isRequired,
    commentCount: PropTypes.number.isRequired,
    showPlus: PropTypes.bool,
    userAuthorized: PropTypes.bool,
    userName: PropTypes.string,
    deleteVoteCallback: PropTypes.func,
    voteCallback: PropTypes.func,
    followCallback: PropTypes.func
}

export default Post;