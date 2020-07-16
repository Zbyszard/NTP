import React, { Component } from 'react';
import { GetPostApiConstants, GetPageCountApiConstants } from '../../Shared/ApiConstants/ApiConstants';
import PostList from '../../components/Posts/PostList/PostList';
import PageControllerContext from '../../Context/PageControllerContext';
import AuthContext from '../../Context/AuthContext';

class LoggedUserPage extends Component {

    render() {
        return (
            <AuthContext.Consumer>
                {authContext =>
                    <PageControllerContext.Consumer>
                        {context =>
                            <PostList
                                showForm={true}
                                getPostsUrl={`${GetPostApiConstants.byUser}/${authContext.userName}/${context.sort}/${context.sortOrder}/${context.postsPerPage}`}
                                getPageCountUrl={`${GetPageCountApiConstants.byUser}/${authContext.userName}/${context.postsPerPage}`} />
                        }
                    </PageControllerContext.Consumer>
                }
            </AuthContext.Consumer>

        );
    }
}

export default LoggedUserPage;