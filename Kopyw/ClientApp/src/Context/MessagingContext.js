import React from 'react';

const MessagingContext = React.createContext({
    conversations: [],
    loadingConversationIds: [],
    activeConversationId: 0,
    areConversationsLoading: false,
    searchResults: [],
    enterConversation: convId => { },
    exitConversation: () => { },
    requestMessages: (convId, olderThan) => { },
    setConversationInput: (convId, value) => { },
    sendMessage: messageObj => { },
    searchConversations: string => { },
    createConversation: conversationObject => { }
});

export default MessagingContext;