type ImagePlaceholderProps = {
  title: string;
  fileName: string;
  description: string;
};

function ImagePlaceholder({ title, fileName, description }: ImagePlaceholderProps) {
  return (
    <figure className="image-placeholder">
      <div className="placeholder-icon">▧</div>
      <figcaption>
        <strong>{title}</strong>
        <span>{description}</span>
        <code>{fileName}</code>
      </figcaption>
    </figure>
  );
}

export default ImagePlaceholder;