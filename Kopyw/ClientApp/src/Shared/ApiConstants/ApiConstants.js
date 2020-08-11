export const GetPostApiConstants = {
    single: "/post",
    new: "/post/time/desc",
    top: "/post/score/desc",
    byUser: "/post/user",
    search: "/post/search",
    observed: "/post/observed"
}

export const GetPageCountApiConstants = {
    all: "/post/pages",
    byUser: "/post/user/pages",
    searched: "/post/search/pages",
    observed: "/post/observed/pages"
}

export const PostApiConstants = {
    add: "/post",
    edit: "/post/edit",
    delete: "/post/delete",
    addVote: "/post/vote",
    deleteVote: "/post/vote",
    getInfo: "/post/info"
}

export const FollowApiConstants = {
    add: "/follow/add",
    delete: "/follow/delete"
}

export const ConversationApiConstants = {
    getConversations: "/conversation/range",
    getConversation: "/conversation",
    getMessages: "/conversation/messages",
    createConversation: "/conversation/create",
    sendMessage: "/conversation/message",
    search: "/conversation/search"
}

export const UserApiConstants = {
    getStats: "/userStats"
}