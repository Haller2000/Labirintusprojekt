type ImagePlaceholderProps = {
  title: string;
  description: string;
  imageSrc: string;
};

function ImagePlaceholder({ title, description, imageSrc }: ImagePlaceholderProps) {
  return (
    <figure className="image-box">
      <img src={imageSrc} alt={title} />

      <figcaption>
        <strong>{title}</strong>
        <span>{description}</span>
      </figcaption>
    </figure>
  );
}

export default ImagePlaceholder;