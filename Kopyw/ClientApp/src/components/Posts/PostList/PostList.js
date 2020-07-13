import React, { Component } from 'react';
import AuthContext from '../../api-authorization/AuthContext';
import AuthorizedRender from '../../api-authorization/AuthorizedRender';
import PropTypes from 'prop-types';
import Post from './Post/Post';
import PostForm from '../PostForm';
import axios from 'axios';
import Communicate from '../../Shared/Communicate/Communicate';
import LoadingSymbol from '../../Shared/LoadingSymbol/LoadingSymbol';
import classes from './PostList.module.css';
import PageSelector from '../PageSelector/PageSelector';
import Warning from '../../Shared/Warning/Warning';

class PostList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            posts: [],
            pageCount: 1,
            postsPerPage: 10,
            currentPage: props.match.params.page || 1,
            sliderValue: 1,
            sliderMax: 1,
            sort: "time",
            sortOrder: "desc",
            lastRequestUrl: null,
            isLoading: true,
            formBlocked: false,
            formKey: new Date().getTime(),
            postIdToBeDeleted: null
        }
        this.dataCancelSource = null;
        this.pageDataCancelSource = null;
        this.justCaughtInfo = false;
        this.formBlockTimer = null;
    }

    componentDidMount = () => {
        this.requestPageCount();
        this.requestData();
    }

    componentWillUnmount = () => {
        clearTimeout(this.formBlockTimer);
        if (this.dataCancelSource)
            this.dataCancelSource.cancel();
        if (this.pageDataCancelSource)
            this.pageDataCancelSource.cancel();
    }

    componentDidUpdate = (prevProps, prevState) => {
        let page = this.props.match.params.page || 1;
        if (prevProps.location.pathname !== this.props.location.pathname) {
            this.changePage(page);
            this.requestPageCount();
            this.requestData();
        }
        if (prevState.posts !== this.state.posts && !this.justCaughtInfo) {
            this.requestPostInfo();
        }
        if (this.justCaughtInfo)
            this.justCaughtInfo = false;
    }

    postVote = postId => {
        const data = { postId: postId };
        axios.post("/post/vote", data).then(response => {
            this.setPostVote(postId, true);
        });
    }

    deletePostVote = postId => {
        axios.delete(`/post/vote/${postId}`).then(response => {
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

    createRequestUrl = () => {
        let getUrl = this.props.getUrl;
        let page = this.props.match.params.page || 1;
        if (getUrl.includes("/post/user")) {
            if (getUrl === "/post/user") {
                let user = this.props.match.params.username;
                getUrl += `/${user}`;
            }
            getUrl += `/${this.state.sort}/${this.state.sortOrder}/${page}/${this.state.postsPerPage}`;
            return getUrl;
        }
        if (getUrl.includes("/search")) {
            getUrl += `/${this.props.match.params.phrase}/${this.state.sort}/` +
                `${this.state.sortOrder}/${page}/${this.state.postsPerPage}`;
            return getUrl;
        }
        if (getUrl.includes("time") || getUrl.includes("score")) {
            getUrl += `/${this.state.sortOrder}/${page}/${this.state.postsPerPage}`;
            return getUrl;
        }

        getUrl += `/${this.state.sort}/${this.state.sortOrder}/${page}/${this.state.postsPerPage}`;
        return getUrl;
    }

    requestData = () => {
        let url = this.createRequestUrl();
        this.setState({ isLoading: true });
        if (this.state.isLoading && this.dataCancelSource)
            this.dataCancelSource.cancel();
        this.dataCancelSource = axios.CancelToken.source();
        axios.get(url, { cancelToken: this.dataCancelSource.token })
            .then(response => {
                this.setState({
                    posts: response.data,
                    isLoading: false,
                    currentPage: this.props.match.params.page || 1,
                    lastRequestUrl: this.props.location.pathname
                });
                this.dataCancelSource = null;
            })
            .catch(error => {
                if (axios.isCancel(error))
                    return;
                console.log(error);
                this.setState({ isLoading: false })
            });
    }

    createPageRequestUrl = () => {
        let getUrl = this.props.getUrl;
        if (getUrl.includes("/post/user")) {
            if (getUrl === "/post/user")
                getUrl += `/pages/${this.props.match.params.username}/${this.state.postsPerPage}`;
            else {
                getUrl = `${getUrl.substring(0, "/post/user".length)}/` +
                    `pages/${getUrl.substring("/post/user".length + 1)}/${this.state.postsPerPage}`;
            }
            return getUrl;
        }
        if (getUrl.includes("/search")) {
            getUrl += `/pages/${this.props.match.params.phrase}/${this.state.postsPerPage}`;
            return getUrl;
        }
        if (getUrl.includes("time") || getUrl.includes("score")) {
            getUrl = `/post/pages/${this.state.postsPerPage}`;
            return getUrl;
        }
        return getUrl + `/pages/${this.state.postsPerPage}`;
    }

    requestPageCount = () => {
        let url = this.createPageRequestUrl();
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
        const ids = this.state.posts.map(p => p.id);
        axios.post("/post/info", ids)
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
                this.justCaughtInfo = true;
                this.setState({ posts: updatedPosts });
            });
    }


    sendNewPost = newPost => {
        this.setState({ formBlocked: true });
        axios.post("/post", newPost)
            .then(r => {
                const post = r.data;
                this.postAdded(post);
            })
            .finally(() => {
                this.formBlockTimer = setTimeout(() => this.setState({ formBlocked: false }), 1000);
            });
    }

    postAdded = post => {
        if (this.state.currentPage === 1)
            this.setState(state => {
                state.posts.pop();
                return {
                    posts: [post, ...state.posts],
                    formKey: new Date().getTime()
                };
            });
        else
            this.changePage(1);
    }

    postEdited = post => {
        axios.put("/post/edit", post)
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
        axios.delete(`/post/delete/${this.state.postIdToBeDeleted}`)
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
        if (url.includes(":username"))
            url = url.replace(":username", this.props.match.params.username);
        else if (url.includes(":phrase"))
            url = url.replace(":phrase", this.props.match.params.phrase);
        return url.substring(0, url.indexOf(':page?'));
    }

    setSliderValue = value => {
        this.setState({ sliderValue: value });
    }

    setSliderMax = value => {
        this.setState({ sliderMax: value });
    }

    changePage = pageNum => {
        if (pageNum < 1)
            pageNum = 1;
        else if (pageNum > this.state.pageCount)
            pageNum = this.state.pageCount;
        this.setState({ currentPage: pageNum });
        this.setSliderValue(pageNum);
    }

    render() {
        let pageSelector;
        let content;
        let loadingSymbol;
        if (this.state.isLoading) {
            loadingSymbol = <Communicate><LoadingSymbol /></Communicate>;
        }
        if (!this.state.isLoading && !this.state.posts.length) {
            content = <Communicate>Nothing to show</Communicate>;
        }
        else if (!!this.state.posts.length) {
            pageSelector =
                <PageSelector pagesCount={this.state.pageCount}
                    currentPage={+this.state.currentPage}
                    url={this.urlWithoutParams()}
                    onLinkClick={this.changePage}
                    max={this.state.sliderMax}
                    value={+this.state.sliderValue}
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
                            followCallback={this.requestPostInfo} />
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
    getUrl: PropTypes.string.isRequired,
    showForm: PropTypes.bool
}

export default PostList;