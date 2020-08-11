const getConversationDisplayName = (conversation, loggedUser) => {
    if (typeof conversation !== typeof {})
        throw new Error("Type error: first argument is not an object");
    if (!conversation.userNames || !Array.isArray(conversation.userNames))
        throw new Error("Type error: invalid conversation object");
    if (typeof loggedUser !== typeof "")
        throw new Error("Type error: second argument is not a string");

    if (conversation.isGroup) {
        if (conversation.name)
            return conversation.name;
        return conversation.userNames.join(", ");
    }
    if (conversation.userNames.length === 1 && conversation.userNames[0] === loggedUser)
        return loggedUser;
    return conversation.userNames.find(name => name !== loggedUser);
}

export default getConversationDisplayName;