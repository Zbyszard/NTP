import React, { Component } from 'react';
import { Route } from 'react-router-dom';
import AuthContext from '../../api-authorization/AuthContext';
import AuthorizedRender from '../../api-authorization/AuthorizedRender';
import PropTypes from 'prop-types';
import Post from './Post/Post';
import PostForm from '../PostForm';
import axios from 'axios';
import classes from './PostList.module.css';
import PageSelector from '../PageSelector/PageSelector';

class PostList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            posts: [],
            pageCount: 1,
            postsPerPage: 10,
            currentPage: props.match.params.page || 1,
            sortDir: "desc",
            isLoading: true
        }
    }

    componentDidMount = () => {
        this.requestPageCount();
        this.requestData(this.state.currentPage);
    }

    requestData = pageNumber => {
        let url = `${this.props.getUrl}/${pageNumber}/${this.state.postsPerPage}`;
        if (this.props.getUrl === "/post/user")
            url = `${this.props.getUrl}/${this.props.match.params.username}/${pageNumber}/${this.state.postsPerPage}`;
        url += `/${this.state.sort}/${this.state.sortDir}`;
        axios.get(url).then(response => {
            this.setState({
                posts: response.data,
                isLoading: false
            });
        })
            .catch(error => {
                console.log(error);
                this.setState({ isLoading: false })
            });
    }

    requestPageCount = () => {
        let url;
        let getUrl = this.props.getUrl;
        let userName;
        let userString = "/post/user";
        if (this.props.getUrl.includes(userString)) {
            userName = getUrl.substring(getUrl.indexOf(userString) + userString.length);
            url = `/post/user/pages/${userName}/${this.state.postsPerPage}`;
        }
        else if (this.props.getUrl.includes("observed"))
            url = `/post/observed/pages/${this.state.postsPerPage}`;
        else
            url = `/post/pages/${this.state.postsPerPage}`;
        axios.get(url).then(r => {
            this.setState({ pageCount: r.data });
            console.log(r.data);
        });
    }

    postAddedCallback = post => {
        if (this.state.currentPage == 1)
            this.setState(state => {
                return { posts: [post, ...state.posts] };
            });
        else
            this.changePage(1);
    }

    urlWithoutParams = () => {
        let url = this.props.match.path;
        if (url.includes(":username"))
            url = url.replace(":username", this.props.match.params.username);
        return url.substring(0, url.indexOf(':page?'));
    }

    changePage = pageNum => {
        if (pageNum < 1)
            pageNum = 1;
        if (pageNum > this.state.pageCount)
            pageNum = this.state.pageCount;
        this.setState({ currentPage: pageNum });
        this.requestData(pageNum);
    }

    render() {
        let pageSelector;
        let content;
        if (this.state.isLoading) {
            content = <div>Loading...</div>;
        }
        else if (!this.state.posts.length) {
            content = <div>Nothing to show</div>;
        }
        else {
            pageSelector =
                <PageSelector pagesCount={this.state.pageCount}
                    currentPage={this.state.currentPage}
                    url={this.urlWithoutParams()}
                    onLinkClick={this.changePage} />
            content = this.state.posts.map(p =>
                <AuthContext.Consumer>
                    {context =>
                        <Post id={p.id} key={p.id}
                            author={p.authorName}
                            title={p.title}
                            postTime={new Date(p.postTime)}
                            text={p.text}
                            score={p.score}
                            commentCount={p.commentCount}
                            userVote={p.userVote}
                            showPlus={context.authorized && context.userName !== p.authorName} />
                    }
                </AuthContext.Consumer>);
        }
        let form = null;
        if (this.props.showForm)
            form =
                <AuthorizedRender>
                    <PostForm onPost={this.postAddedCallback} />
                </AuthorizedRender>;
        return (
            <>
                {form}
                {pageSelector}
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