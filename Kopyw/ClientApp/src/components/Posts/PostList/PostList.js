import React, { Component } from 'react';
import AuthContext from '../../api-authorization/AuthContext';
import PropTypes from 'prop-types';
import Post from './Post/Post';
import axios from 'axios';
import classes from './PostList.module.css';

class PostList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            posts: null,
            postsPerPage: 10,
            currentPage: 1,
            sort: "time",
            sortDir: "desc",
            isLoading: true
        }
    }

    componentDidMount = () => {
        this.requestData();
    }

    requestData = () => {
        let url = `${this.props.getUrl}/${this.state.currentPage}/${this.state.postsPerPage}`;
        if (this.props.getUrl === "/post/user")
            url = `${this.props.getUrl}/${this.props.match.params.username}/${this.state.currentPage}/${this.state.postsPerPage}`;
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

    render() {
        let content;
        if (this.state.isLoading)
            content = <div>Loading...</div>;
        else if (this.state.posts == null)
            content = <div>Nothing to show</div>;
        else
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
        return (
            <div className={classes.postList}>
                {content}
            </div>
        );
    }
}

PostList.propTypes = {
    getUrl: PropTypes.string.isRequired
}

export default PostList;