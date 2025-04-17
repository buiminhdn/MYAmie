// export default function createFormData(data: Record<string, any>): FormData {
//   const formData = new FormData();

//   Object.entries(data).forEach(([key, value]) => {
//     if (value == null) return; // Skip null or undefined values

//     if (Array.isArray(value)) {
//       const isFileArray = key.toLowerCase().includes("file");
//       value.forEach((item, index) => {
//         formData.append(
//           isFileArray ? key : `${key}[${index}]`,
//           isFileArray ? item : item.toString()
//         );
//       });
//     } else {
//       formData.append(key, value.toString());
//     }
//   });

//   return formData;
// }

export default function createFormData(data: Record<string, any>): FormData {
  const formData = new FormData();

  Object.entries(data).forEach(([key, value]) => {
    if (value === null || value === undefined) return; // Skip null & undefined values

    if (Array.isArray(value)) {
      value.forEach((item) => formData.append(key, item)); // Append array items under the same key
    } else {
      formData.append(key, value instanceof File ? value : value.toString()); // Ensure proper file handling
    }
  });

  return formData;
}
