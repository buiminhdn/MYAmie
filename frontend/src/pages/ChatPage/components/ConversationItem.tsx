import { Link } from "react-router-dom";
import { useSelector } from "react-redux";
import { accountIdSelector } from "@/store/auth/auth.selector";
import { ConversationVM } from "@/models/viewmodels/chat.vm";
import Avatar from "@/components/Avatar/Avatar";

interface ConversationItemProps {
  conversation: ConversationVM;
}

function ConversationItem({ conversation }: ConversationItemProps) {
  const currentUserId = useSelector(accountIdSelector);
  return (
    <Link
      to={`/chat/${conversation.id}`}
      target="_blank"
      className="relative w-full flex gap-3 items-center hover:bg-primary-lighter px-2 py-3 rounded-md"
    >
      <Avatar
        src={conversation.avatar}
        hasBorder={false}
        alt="avatar"
        size="size-12"
      />
      <div className="block w-full overflow-hidden">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-2">
            <p className="font-medium">{conversation.name}</p>
            {conversation.senderId !== currentUserId && (
              <div className="text-primary">
                <i className="fa-solid fa-xs fa-circle"></i>
              </div>
            )}
          </div>
          <p className="text-xs text-gray-400">{conversation.sentAt}</p>
        </div>
        <p className="text-gray-600 truncate mt-0.5 text-xs">
          {conversation.senderId === currentUserId ? "Bạn: " : ""}
          {conversation.content}
        </p>
      </div>
    </Link>
  );
}

export default ConversationItem;
