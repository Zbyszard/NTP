import React, { Component } from 'react';
import { withRouter } from 'react-router-dom';
import AuthContext from '../../../Context/AuthContext';
import AuthorizedRender from '../../api-authorization/AuthorizedRender';
import { PostApiConstants } from '../../../Shared/ApiConstants/ApiConstants';
import * as hubConstants from '../../../Shared/SignalR/HubConstants';
import PropTypes from 'prop-types';
import Post from './Post/Post';
import PostForm from '../PostForm';
import axios from 'axios';
import Communicate from '../../Shared/Communicate/Communicate';
import LoadingSymbol from '../../Shared/LoadingSymbol/LoadingSymbol';
import classes from './PostList.module.css';
import PageSelector from '../PageSelector/PageSelector';
import Warning from '../../Shared/Warning/Warning';
import Subscriber from '../../../Shared/SignalR/Subscriber';

class PostList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            posts: [],
            currentPage: 1,
            pageCount: 1,
            sliderValue: 1,
            sliderMax: 1,
            isLoading: true,
            formBlocked: false,
            formKey: new Date().getTime(),
            postIdToBeDeleted: null
        }
        this.dataCancelSource = null;
        this.pageDataCancelSource = null;
        this.justCaughtInfo = false;
        this.formBlockTimer = null;
        this.subscriber = Subscriber.getSubscriber(hubConstants.postHubUrl);
    }

    componentDidMount = () => {
        this.setPage();
        this.requestPageCount();
        this.requestData();
        if (this.subscriber.isConnected)
            this.subscriber.connection.on("UpdateReceived", this.updatePost);
        else
            this.subscriber.addOnReadyCallback(() => {
                this.subscriber.connection.on("UpdateReceived", this.updatePost);
            })
    }

    componentWillUnmount = () => {
        clearTimeout(this.formBlockTimer);
        if (this.dataCancelSource)
            this.dataCancelSource.cancel();
        if (this.pageDataCancelSource)
            this.pageDataCancelSource.cancel();
        if (this.subscriber.isConnected)
            this.subscriber.connection.off("UpdateReceived");
    }

    componentDidUpdate = (prevProps, prevState) => {
        if (prevProps.match.params.page !== this.props.match.params.page)
            this.setPage();
        else if (prevState.currentPage !== this.state.currentPage) {
            this.requestPageCount();
            this.requestData();
        }

        if (prevState.posts !== this.state.posts && !this.justCaughtInfo)
            this.requestPostInfo();
        else if (this.justCaughtInfo)
            this.justCaughtInfo = false;
    }

    subscribeUpdates = (prevPostId, postId) => {
        if (this.subscriber.isConnected) {
            prevPostId && this.subscriber.connection.invoke("Unsubscribe", prevPostId);
            this.subscriber.connection.invoke("Subscribe", postId);
        }
        else {
            this.subscriber.addOnReadyCallback(() => {
                prevPostId && this.subscriber.connection.invoke("Unsubscribe", prevPostId);
                this.subscriber.connection.invoke("Subscribe", postId);
            })
        }
    }

    unsubscribeUpdates = postId => {
        if (this.subscriber.isConnected)
            this.subscriber.connection.invoke("Unsubscribe", postId);
    }

    postVote = postId => {
        let url = PostApiConstants.addVote;
        const data = { postId: postId };
        axios.post(url, data).then(response => {
            if (response.status === 200)
                this.setPostVote(postId, true);
        });
    }

    deletePostVote = postId => {
        let url = `${PostApiConstants.deleteVote}/${postId}`;
        axios.delete(url)
            .then(response => {
                if (response.status === 200)
                    this.setPostVote(postId, false);
            });
    }

    setPostVote = (postId, value) => {
        let posts = this.state.posts;
        let index = posts.findIndex(p => p.id === postId);
        if (index === -1)
            return;
        posts[index].userVote = value;
        this.setState({ posts: posts });
    }

    requestData = () => {
        let url = null;
        if (this.props.singlePostId)
            url = `${this.props.getPostsUrl}/${this.props.singlePostId}`;
        else
            url = `${this.props.getPostsUrl}/${this.state.currentPage}`;
        this.setState({ isLoading: true });
        if (this.dataCancelSource)
            this.dataCancelSource.cancel();
        this.dataCancelSource = axios.CancelToken.source();
        axios.get(url, { cancelToken: this.dataCancelSource.token })
            .then(response => {
                let data = this.props.singlePostId ? [response.data] : response.data;
                this.setState({
                    posts: data,
                    isLoading: false,
                    lastRequestUrl: this.props.location.pathname
                });
                this.dataCancelSource = null;
                window.scroll(0, 0);
            })
            .catch(error => {
                if (axios.isCancel(error))
                    return;
                console.log(error);
                this.setState({ isLoading: false })
            });
    }

    requestPageCount = () => {
        if (this.props.singlePostId)
            return;
        let url = this.props.getPageCountUrl;
        if (this.state.isLoading && this.pageDataCancelSource)
            this.pageDataCancelSource.cancel();
        this.pageDataCancelSource = axios.CancelToken.source();
        axios.get(url, { cancelToken: this.pageDataCancelSource.token })
            .then(r => {
                this.setState({ pageCount: r.data });
                this.pageDataCancelSource = null;
            })
            .catch(error => {
                if (axios.isCancel(error))
                    return;
            });
    }

    requestPostInfo = () => {
        let url = PostApiConstants.getInfo;
        const ids = this.state.posts.map(p => p.id);
        axios.post(url, ids)
            .then(r => {
                const d = r.data;
                const posts = this.state.posts;
                let updatedPosts = d.map((inf, index) => {
                    if (inf.postId === posts[index].id) {
                        let p = posts[index];
                        p.commentCount = inf.commentCount;
                        p.score = inf.score;
                        p.userVote = inf.userVote;
                        p.followingAuthor = inf.followingAuthor;
                        return p;
                    }
                    return null;
                });
                if (updatedPosts.some(p => p === null)) {
                    this.justCaughtInfo = false;
                    return;
                }
                this.justCaughtInfo = true;
                this.setState({ posts: updatedPosts });
            });
    }


    sendNewPost = newPost => {
        let url = PostApiConstants.add;
        this.setState({ formBlocked: true });
        axios.post(url, newPost)
            .then(response => {
                if (response.status === 201) {
                    this.setState({ formKey: new Date().getTime() });
                    if (this.state.currentPage === 1)
                        this.requestData();
                    else
                        this.props.history.push(this.props.match.url);
                }
            })
            .finally(() => {
                this.formBlockTimer = setTimeout(() => this.setState({ formBlocked: false }), 1000);
            });
    }

    postEdited = post => {
        let url = PostApiConstants.edit;
        axios.put(url, post)
            .then(r => {
                if (r.status === 200) {
                    let edited = r.data;
                    let posts = this.state.posts;
                    let index = posts.findIndex(p => p.id === edited.id);
                    posts[index].text = edited.text;
                    posts[index].title = edited.title;
                    posts[index].lastEditTime = edited.lastEditTime;
                    this.setState({ posts: posts });
                }
            })
    }

    showDeleteWarning = postId => {
        this.setState({ postIdToBeDeleted: postId });
    }

    deletePost = () => {
        let url = `${PostApiConstants.delete}/${this.state.postIdToBeDeleted}`;
        axios.delete(url)
            .then(r => {
                if (r.status === 200) {
                    this.requestData();
                    this.setState({ postIdToBeDeleted: null });
                }
            })
    }

    cancelDeletePost = () => {
        this.setState({ postIdToBeDeleted: null });
    }

    urlWithoutParams = () => {
        let url = this.props.match.path;
        for (let key in this.props.match.params) {
            if (key === "page") {
                url = url.replace("/:page?", '');
                continue;
            }
            url = url.replace(`:${key}`, this.props.match.params[key]);
        }
        return url;
    }

    setSliderMax = value => {
        this.setState({ sliderMax: value });
    }

    setPage = () => {
        let page = !isNaN(+this.props.match.params.page) && +this.props.match.params.page || 1;
        this.setState({ currentPage: page });
    }

    updatePost = update => {
        let posts = [...this.state.posts];
        let post = posts.find(p => p.id === update.postId);
        if (post) {
            post.score = update.score;
            post.commentCount = update.commentCount;
            this.setState({ posts: posts });
        }
    }

    render() {
        let pageSelector = null;
        let content;
        let loadingSymbol;
        if (this.state.isLoading) {
            loadingSymbol = <Communicate zIndex={-1}><LoadingSymbol /></Communicate>;
        }
        if (!this.state.isLoading && !this.state.posts.length) {
            content = <Communicate zIndex={-1}>Nothing to show</Communicate>;
        }
        else if (!!this.state.posts.length) {
            if (!this.props.singlePostId && this.state.pageCount !== 1)
                pageSelector =
                    <PageSelector pagesCount={this.state.pageCount}
                        currentPage={+this.state.currentPage}
                        url={this.urlWithoutParams()}
                        max={this.state.sliderMax}
                        value={this.state.currentPage}
                        setMax={this.setSliderMax}
                        setValue={this.setSliderValue} />
            content = this.state.posts.map(p =>
                <AuthContext.Consumer key={p.id}>
                    {context =>
                        <Post id={p.id}
                            author={p.authorName}
                            authorId={p.authorId}
                            title={p.title}
                            postTime={new Date(p.postTime)}
                            lastEdit={p.lastEditTime ? new Date(p.lastEditTime) : null}
                            text={p.text}
                            score={p.score}
                            commentCount={p.commentCount}
                            userVote={p.userVote}
                            followingAuthor={p.followingAuthor}
                            showPlus={context.authorized && context.userName !== p.authorName}
                            userAuthorized={context.authorized}
                            userName={context.userName}
                            editCallback={this.postEdited}
                            deleteCallback={this.showDeleteWarning}
                            voteCallback={this.postVote}
                            deleteVoteCallback={this.deletePostVote}
                            followCallback={this.requestPostInfo}
                            subscribeCallback={this.subscribeUpdates}
                            unsubscribeCallback={this.unsubscribeUpdates} />
                    }
                </AuthContext.Consumer>);
        }
        let form = null;
        if (this.props.showForm)
            form =
                <AuthorizedRender>
                    <PostForm key={this.state.formKey}
                        postCallback={this.sendNewPost}
                        isBlocked={this.state.formBlocked} />
                </AuthorizedRender>;
        let deleteWarning = this.state.postIdToBeDeleted ?
            <Warning message="Are you sure you want to delete this post?"
                confirmCallback={this.deletePost}
                cancelCallback={this.cancelDeletePost} /> :
            null;
        return (
            <>
                {deleteWarning}
                {form}
                {pageSelector}
                {loadingSymbol}
                <div className={classes.postList}>
                    {content}
                </div>
                {pageSelector}
            </>
        );
    }
}

PostList.propTypes = {
    getPostsUrl: PropTypes.string.isRequired,
    getPageCountUrl: PropTypes.string.isRequired,
    showForm: PropTypes.bool,
    singlePostId: PropTypes.bool
}

export default withRouter(PostList);