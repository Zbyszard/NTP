import React, { Component } from 'react';
import { GetPostApiConstants, GetPageCountApiConstants } from '../../Shared/ApiConstants/ApiConstants';
import PostList from '../../components/Posts/PostList/PostList';
import PageControllerContext from '../../Context/PageControllerContext';

class ObservedPage extends Component {
    render() {
        return (
            <PageControllerContext.Consumer>
                {context =>
                    <PostList
                        getPostsUrl={`${GetPostApiConstants.observed}/${context.sort}/${context.sortOrder}/${context.postsPerPage}`}
                        getPageCountUrl={`${GetPageCountApiConstants.observed}/${context.postsPerPage}`} />
                }
            </PageControllerContext.Consumer>
        );
    }
}

export default ObservedPage;