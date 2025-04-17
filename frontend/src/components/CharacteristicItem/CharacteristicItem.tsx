interface CharacteristicItemProps {
  text: string;
}

function CharacteristicItem({ text }: CharacteristicItemProps) {
  return (
    <div className="flex items-center gap-2 break-all py-1 px-3 rounded-full border-2 border-gray-300">
      <span>{text}</span>
    </div>
  );
}

export default CharacteristicItem;
