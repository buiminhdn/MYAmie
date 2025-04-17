import { useEffect, useRef, RefObject } from "react";

const useClickOutside = <T extends HTMLElement>(
  handler: () => void,
  events: Array<"mousedown" | "mouseup" | "touchstart"> = ["mousedown"]
): RefObject<T> => {
  const ref = useRef<T>(null);

  useEffect(() => {
    const handleEvent = (event: MouseEvent | TouchEvent) => {
      if (!ref.current?.contains(event.target as Node)) {
        handler();
      }
    };

    events.forEach((event) => document.addEventListener(event, handleEvent));

    return () =>
      events.forEach((event) =>
        document.removeEventListener(event, handleEvent)
      );
  }, [handler, events]);

  return ref as RefObject<T>;
};

export default useClickOutside;
