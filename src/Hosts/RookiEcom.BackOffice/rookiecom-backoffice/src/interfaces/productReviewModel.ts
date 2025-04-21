export default interface IProductReview {
  id?: string;
  productId: number;
  customerId: string;
  score: number;
  content: string;
  image?: string;
}