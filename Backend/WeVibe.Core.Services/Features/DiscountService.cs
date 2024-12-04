using AutoMapper;
using WeVibe.Core.Contracts.Discount;
using WeVibe.Core.Domain.Entities;
using WeVibe.Core.Domain.Repositories;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.Core.Services.Features
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository discountRepository, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        public async Task<DiscountDto> CreateDiscountAsync(CreateDiscountDto discountDto)
        {
            var discount = _mapper.Map<Discount>(discountDto);
            await _discountRepository.AddAsync(discount);
            return _mapper.Map<DiscountDto>(discount);
        }

        public async Task<bool> DeleteAsync(int discountId)
        {
            var discount = await _discountRepository.GetByIdAsync(discountId);
            if (discount == null) throw new KeyNotFoundException("Discount not found");

            await _discountRepository.DeleteAsync(discountId);
            return true;
        }

        public async Task<IEnumerable<DiscountDto>> GetAllDiscountsAsync()
        {
            var discounts = await _discountRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DiscountDto>>(discounts);
        }

        public async Task<DiscountDto> GetDiscountByIdAsync(int discountId)
        {
            var discount = await _discountRepository.GetByIdAsync(discountId);
            if (discount == null) throw new KeyNotFoundException("Discount not found");

            return _mapper.Map<DiscountDto>(discount);
        }

        public async Task<DiscountDto> UpdateDiscountAsync(int discountId, DiscountDto discountDto)
        {
            var discount = await _discountRepository.GetByIdAsync(discountId);
            if (discount == null) throw new KeyNotFoundException("Discount not found");

            _mapper.Map(discountDto, discount);
            await _discountRepository.UpdateAsync(discount);
            return _mapper.Map<DiscountDto>(discount);
        }
    }
}
