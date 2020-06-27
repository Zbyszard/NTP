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
            sortDir: "desc",
            lastRequestUrl: null,
            isLoading: true
        }
    }

    componentDidMount = () => {
        this.requestPageCount();
        this.requestData(this.state.currentPage);
    }

    componentDidUpdate = (prevProps, prevState) => {
        let page = this.props.match.params.page || 1;
        if (prevProps.location.pathname !== this.props.location.pathname) {
            this.requestPageCount();
            this.requestData(page);
            this.changePage(page);
        }
    }

    requestData = pageNumber => {
        let getUrl = this.props.getUrl;
        let url;
        url = `${getUrl}/${pageNumber}/${this.state.postsPerPage}`;
        if (this.props.getUrl === "/post/user")
            url = `${getUrl}/${this.props.match.params.username}/${pageNumber}/${this.state.postsPerPage}`;
        url += `/${this.state.sort}/${this.state.sortDir}`;
        if (getUrl.includes("search"))
            url = `${getUrl}/${this.props.match.params.phrase}/${pageNumber}/${this.state.postsPerPage}`;
        this.setState({ isLoading: true });
        axios.get(url).then(response => {

            this.setState({
                posts: response.data,
                isLoading: false,
                currentPage: this.props.match.params.page || 1,
                lastRequestUrl: this.props.location.pathname
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
        let userString = "/user";
        if (getUrl.includes(userString)) {
            let userName;
            userName = this.props.location.pathname.substring(userString.indexOf(userString) + userString.length + 1);
            url = `/post/user/pages/${userName}/${this.state.postsPerPage}`;
        }
        else if (getUrl.includes("search")) {
            url = `/post/search/pages/${this.props.match.params.phrase}/${this.state.postsPerPage}`;
        }
        else if (getUrl.includes("observed"))
            url = `/post/observed/pages/${this.state.postsPerPage}`;
        else
            url = `/post/pages/${this.state.postsPerPage}`;
        axios.get(url).then(r => {
            this.setState({ pageCount: r.data });
        });
    }

    postAddedCallback = post => {
        if (this.state.currentPage === 1)
            this.setState(state => {
                state.posts.pop();
                return { posts: [post, ...state.posts] };
            });
        else
            this.changePage(1);
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