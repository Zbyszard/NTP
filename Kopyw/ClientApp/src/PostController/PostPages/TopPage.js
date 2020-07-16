import React, { Component } from 'react';
import { GetPostApiConstants, GetPageCountApiConstants } from '../../Shared/ApiConstants/ApiConstants';
import PostList from '../../components/Posts/PostList/PostList';
import PageControllerContext from '../../Context/PageControllerContext';

class TopPage extends Component {
    render() {
        return (
            <PageControllerContext.Consumer>
                {context =>
                    <PostList
                        getPostsUrl={`${GetPostApiConstants.top}/${context.postsPerPage}`}
                        getPageCountUrl={`${GetPageCountApiConstants.all}/${context.postsPerPage}`} />
                }
            </PageControllerContext.Consumer>
        );
    }
}

export default TopPage;