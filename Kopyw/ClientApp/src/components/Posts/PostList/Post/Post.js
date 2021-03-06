import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { follow, unfollow } from '../../../../Shared/FollowApiCalls/FollowApiCalls';
import PropTypes from 'prop-types';
import CommentSection from '../../../Comments/CommentSection';
import ContextMenu from '../../../Shared/ContextMenu/ContextMenu';
import ContextMenuItem from '../../../Shared/ContextMenu/ContextMenuItem';
import formatDate from '../../../../Shared/Functions/formatDate';
import classes from './Post.module.css';
import PostForm from '../../PostForm';
import Button from '../../../Shared/Button/Button';
import UserLink from '../../../Shared/UserLink/UserLink';
import { GetPostApiConstants } from '../../../../Shared/ApiConstants/ApiConstants';

class Post extends Component {
    state = {
        showComments: false,
        editMode: false
    }

    render() {
        let plusClasses = [classes.plus];
        if (this.props.userVote)
            plusClasses.push(classes.active);
        const plusClassList = plusClasses.join(' ');
        const plus1 = this.props.showPlus ?
            <button className={plusClassList} onFocus={this.plusOnFocus}
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
        let postBody = null;
        if (this.state.editMode) {
            postBody = <PostForm isBlocked={false}
                title={this.props.title}
                text={this.props.text}
                postCallback={this.editHandler}
                cancelCallback={this.cancelEdit} />
        }
        else {
            postBody =
                <>
                    <div className={classes.header}>
                        <UserLink user={this.props.author} />
                        {menu}
                    </div>
                    <h1 className={classes.title}>
                        <Link to={`${GetPostApiConstants.single}/${this.props.id}`}>
                            {this.props.title}
                        </Link>
                    </h1>
                    <p className={classes.time}
                        title={this.props.lastEdit ? `Last edit: ${formatDate(this.props.lastEdit)}` : null}>
                        {formatDate(this.props.postTime)}
                    </p>
                    <p className={classes.text}>
                        {this.props.text}
                    </p>
                </>
        }
        return (
            <>
                <div className={classes.post}>
                    {postBody}
                    <div className={classes.actionSection}>
                        <span className={classes.score}>+{this.props.score}</span>
                        {plus1}
                        <Button
                            onClick={this.toggleComments}>
                            {`${this.props.commentCount} ${this.props.commentCount === 1 ? "comment" : "comments"}`}
                        </Button>
                    </div>
                    <CommentSection postId={this.props.id}
                        show={this.state.showComments} />
                </div>
            </>
        );
    }

    componentDidMount = () => {
        this.props.subscribeCallback(0, this.props.id);
    }

    componentDidUpdate = (prevProps, prevState) => {
        if(prevProps.id !== this.props.id)
            this.props.subscribeCallback(prevProps.id, this.props.id);
    }

    componentWillUnmount = () => {
        this.props.unsubscribeCallback(this.props.id);
    }

    plusOnFocus = e => {
        e.target.blur();
    }

    getMenuItems = () => {
        const items = [];
        let index = 0;
        if (this.props.author === this.props.userName) {
            items.push(<ContextMenuItem key={index++} clickCallback={this.editItemClicked}>Edit</ContextMenuItem>);
            items.push(<ContextMenuItem key={index++} clickCallback={this.deleteItemClicked}>Delete</ContextMenuItem>);
        }
        else
            items.push(<ContextMenuItem key={index++} clickCallback={this.followItemClicked}>
                {this.props.followingAuthor ? "Unfollow user" : "Observe user"}
            </ContextMenuItem>);
        return items;
    }

    editItemClicked = () => {
        this.setState({ editMode: true });
    }

    editHandler = post => {
        post.id = this.props.id;
        post.authorId = this.props.authorId;
        this.props.editCallback(post);
        this.setState({ editMode: false });
    }

    cancelEdit = () => {
        this.setState({ editMode: false });
    }

    deleteItemClicked = () => {
        this.props.deleteCallback(this.props.id);
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
    followCallback: PropTypes.func,
    subscribeCallback: PropTypes.func,
    unsubscribeCallback: PropTypes.func
}

export default Post;