import React from 'react';

const PostsPerPageContext = React.createContext({
    postsPerPage:10,
    sort: "time",
    sortOrder: "desc"
 });

export default PostsPerPageContext;