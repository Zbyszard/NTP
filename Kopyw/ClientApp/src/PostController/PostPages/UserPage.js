import React, { Component } from 'react';
import { GetPostApiConstants, GetPageCountApiConstants } from '../../Shared/ApiConstants/ApiConstants';
import PostList from '../../components/Posts/PostList/PostList';
import PageControllerContext from '../../Context/PageControllerContext';
import UserStats from '../../components/User/UserStats';

class UserPage extends Component {

    componentDidMount() {
        if (!this.props.match.params.username)
            this.props.history.replace('/');
    }

    render() {
        let user = this.props.match.params.username;
        return (
            <>
                <UserStats userName={user} />
                <PageControllerContext.Consumer>
                    {context =>
                        <PostList key={user}
                            getPostsUrl={`${GetPostApiConstants.byUser}/${user}/${context.sort}/${context.sortOrder}/${context.postsPerPage}`}
                            getPageCountUrl={`${GetPageCountApiConstants.byUser}/${user}/${context.postsPerPage}`} />
                    }
                </PageControllerContext.Consumer>
            </>
        );
    }
}

export default UserPage;